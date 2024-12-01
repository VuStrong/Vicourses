import Config from "../config";
import { AppError } from "../utils/app-error";

type UserAccessTokenResponse = {
    scope: string;
    accessToken: string;
    tokenType: string;
    refreshToken: string;
}

type PaypalUser = {
    userId: string;
    payerId: string;
    email: string;
    emailVerified: boolean;
    verifiedAccount: boolean;
}

export async function getUserAccessToken(authorizationCode: string): Promise<UserAccessTokenResponse> {
    const config = Config.Paypal;
    const auth = Buffer.from(`${config.ClientID}:${config.ClientSecret}`).toString("base64");

    const res = await fetch(`${config.Base}/v1/oauth2/token`, {
        method: "POST",
        body: `grant_type=authorization_code&code=${authorizationCode}`,
        headers: {
            Authorization: `Basic ${auth}`,
        },
    });

    const data = await res.json();

    if (!res.ok) {
        if (data.error === "invalid_authz_code") {
            throw new AppError(data.error_description, 401);
        }

        throw new Error("Cannot get paypal user access token");
    }

    return {
        scope: data.scope,
        accessToken: data.access_token,
        tokenType: data.token_type,
        refreshToken: data.refresh_token,
    };
}

export async function getUserInfo(accessToken: string): Promise<PaypalUser> {
    const config = Config.Paypal;
    const schema = "openid";

    const res = await fetch(
        `${config.Base}/v1/identity/openidconnect/userinfo?schema=${schema}`,
        {
            method: "GET",
            headers: {
                Authorization: `Bearer ${accessToken}`,
                'Content-Type': 'application/json',
            },
        }
    );
    
    if (!res.ok) {
        if (res.status === 401) {
            throw new AppError("Invalid access token", 401);
        }

        throw new Error("Cannot get paypal user info");
    }

    const data = await res.json();
    
    if (typeof data.email_verified === "string") {
        data.email_verified = data.email_verified === "true" ? true : false;
    }
    if (typeof data.verified_account === "string") {
        data.verified_account = data.verified_account === "true" ? true : false;
    }

    return {
        userId: data.user_id,
        payerId: data.payer_id,
        email: data.email,
        emailVerified: data.email_verified,
        verifiedAccount: data.verified_account,
    };
}