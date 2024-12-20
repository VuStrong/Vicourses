import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Category } from "../../types/category";
import axiosInstance from "../../libs/axios";
import Loader from "../../components/Loader";

export default function CategoryEditPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [category, setCategory] = useState<Category>();

    const params = useParams();

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const response = await axiosInstance.get<Category>(
                `/api/cs/v1/categories/${params.slug}`,
            );

            setCategory(response.data);
            setIsLoading(false);
        })();
    }, [params.slug]);

    return (
        <div>
            {isLoading ? (
                <div className="flex justify-center">
                    <Loader />
                </div>
            ) : (
                <div>{category?.name}</div>
            )}
        </div>
    );
}
