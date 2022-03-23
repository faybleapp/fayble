import { Book } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";
import { useApiQuery } from "./useApiQuery";

export const useBook = (id: string) =>
	useApiQuery<Book>(["book", id], `/books/${id}`);

export const useUpdateBook = () => {
	const queryClient = useQueryClient();
	return useApiMutation<Book, string, Book>(
		"PATCH",
		(id) => `/books/${id}`,
		{
			onSuccess: (_, [variables, data]) => {
				queryClient.invalidateQueries("book")
				queryClient.invalidateQueries("books")
			},
		}
	)
}