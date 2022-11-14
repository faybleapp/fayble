import { Book, Series } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiQuery } from "services/useApiQuery";
import { useApiMutation } from "./useApiMutation";

export const useAllSeries = () => useApiQuery<Series[]>(["series"], `/series`);

export const useSeries = (id: string) =>
  useApiQuery<Series>(["series", id], `/series/${id}`);

export const useSeriesBooks = (id: string) =>
  useApiQuery<Book[]>(["series", id, "books"], `/series/${id}/books`);

export const useUpdateSeries = (id: string) => {
  const queryClient = useQueryClient();
  return useApiMutation<Series, Series>("PATCH", () => `/series/${id}`, {
    onSuccess: () => {
      queryClient.invalidateQueries("series");
    },
  });
};

export const useRefreshMetadata = (id: string) => {
  const queryClient = useQueryClient();
  return useApiMutation<null, null>(
    "POST",
    () => `/series/${id}/refresh-metadata`,
    {
      onSuccess: () => {
        queryClient.invalidateQueries(["series", id]);
      },
    }
  );
};
