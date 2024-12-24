"use client"

import { useEffect, useState } from "react";
import { useSession } from "next-auth/react";

import { User } from "@/libs/types/user";
import { getAuthenticatedUser } from "@/services/api/user";
import { Loader } from "@/components/common";
import PaypalLoginButton from "./PaypalLoginButton";

export default function PaypalPayoutMethod() {
    const [user, setUser] = useState<User | null>(null);
    const { data: session } = useSession();

    useEffect(() => {
        (async () => {
            if (session?.accessToken) {
                const result = await getAuthenticatedUser(
                    session.accessToken,
                    "id,paypalAccount"
                );
                
                setUser(result);
            }
        })();
    }, [session?.accessToken]);

    return (
        <div className="flex items-center md:justify-between md:flex-row flex-col justify-center flex-wrap border border-gray-900 p-3">
            <div>
                <img
                    className="w-20 object-cover object-center"
                    src="/img/paypal-logo.png"
                    alt="paypal"
                />
            </div>

            <div>
                {user ? (
                    user.paypalAccount ? (
                        <div className="flex flex-wrap gap-3 items-center justify-center">
                            <div className="flex flex-col items-center">
                                <div className="text-success">
                                    Connected
                                </div>
                                <div className="text-gray-700 truncate max-w-[250px]">
                                    {user.paypalAccount.email}
                                </div>
                            </div>
                            <PaypalLoginButton />
                        </div>
                    ) : (
                        <div className="flex flex-wrap gap-3 items-center justify-center">
                            <div className="text-gray-700">Not connected</div>
                            <PaypalLoginButton />
                        </div>
                    )
                ) : (
                    <Loader />
                )}
            </div>
        </div>
    );
}
