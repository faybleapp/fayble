import { Book, RelatedBooks } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";
import { useApiQuery } from "./useApiQuery";

export const useBook = (id: string) =>
	useApiQuery<Book>(["book", id], `/books/${id}`);

export const useRelatedBooks = (id: string) =>
	useApiQuery<RelatedBooks>(["book", id, "related"], `/books/${id}/related`);

export const useUpdateBook = () => {
	const queryClient = useQueryClient();
	return useApiMutation<Book, string, Book>("PATCH", (id) => `/books/${id}`, {
		onSuccess: () => {
			queryClient.invalidateQueries("book");
			queryClient.invalidateQueries("books");
		},
	});
};
