import { Library, Series } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";
import { useApiQuery } from "./useApiQuery";

export const useLibrary = (id: string) =>
	useApiQuery<Library>(["library", id], `/libraries/${id}`);

export const useLibraries = () =>
	useApiQuery<Library[]>(["libraries"], "/libraries");

export const useLibrarySeries = (id: string) =>
	useApiQuery<Series[]>(["library", id, "series"], `/libraries/${id}/series`);

export const useCreateLibrary = () => {
	const queryClient = useQueryClient();
	return useApiMutation<Library, null, Library>(
		"POST",
		() => `/libraries`,
		{
			onSuccess: () => {
				queryClient.invalidateQueries("libraries");
			},
		}
	);
};

export const useUpdateLibrary = () => {
    const queryClient = useQueryClient();
    return useApiMutation<Library, string, Library>("PATCH", (id: string) => `/libraries/${id}`,
    {
        onSuccess: () => {
            queryClient.invalidateQueries("library")
            queryClient.invalidateQueries("libraries")
        }
    });
};

export const useDeleteLibrary = () => {
	const queryClient = useQueryClient();
	return useApiMutation<null, string, null>(
		"DELETE",
		(id) => `/libraries/${id}`,
		{
			onSuccess: () => {
				queryClient.invalidateQueries("libraries");
			},
		}
	);
};
