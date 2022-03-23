import { BackgroundTaskRequest } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";

export const useRunBackgroundTask = () => {
	const queryClient = useQueryClient();
	return useApiMutation<null, null, BackgroundTaskRequest>(
		"POST",
		() => `/backgroundtasks`,
		{
			onSuccess: () => {
				queryClient.invalidateQueries("backgroundTasks");
			},
		}
	);
};
