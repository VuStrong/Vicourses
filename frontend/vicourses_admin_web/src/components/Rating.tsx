import { FaStar, FaRegStar } from "react-icons/fa";

export default function Rating({
    value,
}: {
    value: number,
}) {
    const renderStars = () => {
        const star = Math.floor(value);
        const stars = [];

        for (let index = 0; index < star; index++) {
            if (stars.length === 5) return stars;
            
            stars.push(<FaStar key={stars.length} />);
        }

        const missing = 5 - stars.length;
        if (missing > 0) {
            for (let index = 0; index < missing; index++) {
                stars.push(<FaRegStar key={stars.length} />);
            }
        }

        return stars;
    }

    return (
        <div className="flex gap-1 items-center text-yellow-400">
            {renderStars()}
        </div>
    )
}