import { Metadata } from "next";
import PaymentsContainer from "./_components/PaymentsContainer";

export const metadata: Metadata = {
    title: "Purchase History",
};

export default function PurchaseHistoryPage() {
    return (
        <div>
            <h1 className="text-gray-900 text-3xl">Purchase History</h1>

            <PaymentsContainer />
        </div>
    );
}
