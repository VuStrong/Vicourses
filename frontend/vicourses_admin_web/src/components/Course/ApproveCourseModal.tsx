import { useState } from "react";

import Button from "../Button";
import { Modal, ModalBody, ModalFooter, ModalHeader } from "../Modal";

export default function ApproveCourseModal({
    open,
    onClose,
    submit,
}: {
    open: boolean;
    onClose: () => void;
    submit: () => Promise<boolean>;
}) {
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const onSubmit = async () => {
        setIsSubmitting(true);

        const success = await submit();

        if (success) onClose();
        setIsSubmitting(false);
    };

    return (
        <Modal open={open} onClose={onClose}>
            <ModalHeader>Approve course</ModalHeader>
            <ModalBody>Approve and publish this course?</ModalBody>
            <ModalFooter>
                <Button onClick={onClose} variant="text">
                    Cancel
                </Button>
                <Button onClick={onSubmit} loading={isSubmitting}>
                    Confirm
                </Button>
            </ModalFooter>
        </Modal>
    );
}
