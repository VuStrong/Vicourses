import PaypalPayoutMethod from "./PaypalPayoutMethod";

export default function ProfilePayoutsPage() {
    return (
        <div>
            <h1 className="text-gray-900 text-3xl">Payouts</h1>

            <div className="mt-5 w-full">
                <div className="text-gray-900">Choose your payout method below</div>

                <div className="flex flex-col gap-5 my-5">
                    <PaypalPayoutMethod />
                </div>
            </div>
        </div>
    )
}