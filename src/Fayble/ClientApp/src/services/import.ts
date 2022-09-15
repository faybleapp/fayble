import { ComicFile } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";

export const useScanImportFiles = () => {
	const queryClient = useQueryClient();
	return useApiMutation<ComicFile[], null, {path: string}>(
		"POST",
		() => `/import/scan`,
		{
			onSuccess: () => {
				queryClient.invalidateQueries("import");
			},
		}
	);
};
