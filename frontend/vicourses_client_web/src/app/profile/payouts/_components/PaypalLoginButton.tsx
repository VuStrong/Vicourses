import { FaPaypal } from "react-icons/fa";

export default function PaypalLoginButton() {
    const authorizationUrl =
        process.env.NEXT_PUBLIC_PAYPAL_LOGIN_AUTHORIZATION_URL;
    const clientId = process.env.NEXT_PUBLIC_PAYPAL_CLIENT_ID;
    const returnUrl = process.env.NEXT_PUBLIC_PAYPAL_LOGIN_RETURN_URL;
    const scope = "email https://uri.paypal.com/services/paypalattributes";

    return (
        <a
            href={`${authorizationUrl}?client_id=${clientId}&response_type=code&scope=${scope}&redirect_uri=${returnUrl}`}
            className="bg-blue-800 text-white rounded-full py-[6px] px-[48px] flex items-center justify-center gap-[9px-]"
        >
            <FaPaypal size={18} />
            <div>Log in with Paypal</div>
        </a>
    );
}
