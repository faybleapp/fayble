import { Book, RelatedBooks } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";
import { useApiQuery } from "./useApiQuery";

export const useBook = (id: string) =>
  useApiQuery<Book>(["book", id], `/books/${id}`);

export const useRelatedBooks = (id: string) =>
  useApiQuery<RelatedBooks>(["book", id, "related"], `/books/${id}/related`);

export const useUpdateBook = (bookId: string) => {
  const queryClient = useQueryClient();
  return useApiMutation<Book, Book>("PATCH", () => `/books/${bookId}`, {
    onSuccess: () => {
      queryClient.invalidateQueries("book");
      queryClient.invalidateQueries("books");
    },
  });
};

export const useRefreshBookMetadata = (id: string) => {
  const queryClient = useQueryClient();
  return useApiMutation<null, null>(
    "POST",
    () => `/book/${id}/refresh-metadata`,
    {
      onSuccess: () => {
        queryClient.invalidateQueries(["book", id]);
      },
    }
  );
};
