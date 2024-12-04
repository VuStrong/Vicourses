"use client";

import { useState } from "react";
import { sendEmailConfirmationLink } from "@/services/api/auth";
import { toast } from "react-hot-toast";
import { Button } from "@material-tailwind/react";

export default function SendEmailConfirmationButton({
    email,
}: {
    email: string;
}) {
    const [isSending, setIsSending] = useState<boolean>(false);

    const handleSendEmailConfirmation = async () => {
        setIsSending(true);

        try {
            await sendEmailConfirmationLink(email);
            toast.success("Email has been sent");
        } catch (error: any) {
            toast.error(error.message);
        }

        setIsSending(false);
    };

    return (
        <Button
            loading={isSending}
            onClick={handleSendEmailConfirmation}
            className="bg-primary"
        >
            Resend email confirmation link
        </Button>
    );
}