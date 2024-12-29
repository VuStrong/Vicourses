import fs from "fs";
import { getSignedUrl } from "@aws-sdk/cloudfront-signer";
import jwt from "jsonwebtoken";
import Config from "../config";
import { AppError } from "../utils/app-error";

export async function getHlsManifestUrl(mediaToken: string): Promise<{
    manifestFileUrl: string;
    params: string;
}> {
    let tokenPayload: any;

    try {
        tokenPayload = jwt.verify(
            mediaToken,
            Config.MediaFileSecret || ""
        ) as any;
    } catch (error) {
        throw new AppError("Media token invalid", 403);
    }

    const manifestFileId = tokenPayload.manifestFileId as string;
    const manifestDir = manifestFileId.replace("/master.m3u8", "");
    const privateKey = fs.readFileSync(Config.Cloudfront.PrivateKeyPath || "");

    const signedUrl = getSignedUrl({
        keyPairId: Config.Cloudfront.KeyPairId || "",
        privateKey,
        policy: JSON.stringify({
            Statement: [
                {
                    Resource: `${Config.Cloudfront.Domain}/${manifestDir}/*`,
                    Condition: {
                        DateLessThan: {
                            "AWS:EpochTime": Math.round(
                                new Date(
                                    new Date().getTime() + 60 * 60 * 1000
                                ).getTime() / 1000
                            ),
                        },
                    },
                },
            ],
        }),
    });

    const cloudfrontUrlParams = signedUrl.split("?")[1];

    return {
        manifestFileUrl: `${Config.Cloudfront.Domain}/${manifestFileId}`,
        params: cloudfrontUrlParams,
    };
}
