import { Book, Series } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiQuery } from "services/useApiQuery";
import { useApiMutation } from "./useApiMutation";

export const useSeries = (id: string) =>
	useApiQuery<Series>(["series", id], `/series/${id}`);

export const useSeriesBooks = (id: string) =>
	useApiQuery<Book[]>(["series", id, "books"], `/series/${id}/books`);

export const useUpdateSeries = () => {
	const queryClient = useQueryClient();
	return useApiMutation<Series, string, Series>(
		"PATCH",
		(id) => `/series/${id}`,
		{
			onSuccess: (_, [variables, data]) => {
				queryClient.invalidateQueries("series");
			},
		}
	);
};
