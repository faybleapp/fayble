import { FirstRun, SystemConfiguration } from "models/api-models";
import { useQueryClient } from "react-query";
import { toast } from "react-toastify";
import { useApiMutation } from "./useApiMutation";
import { useApiQuery } from "./useApiQuery";

export const useFirstRun = () => {
	const queryClient = useQueryClient();
	return useApiMutation<null, null, FirstRun>(
		"POST",
		() => `/system/first-run`,
		{
			onSuccess: () => {
				queryClient.removeQueries("systemConfiguration");
				queryClient.invalidateQueries("systemConfiguration");
			},
		}
	);
};

export const useSystemConfiguration = () =>
	useApiQuery<SystemConfiguration>(
		["systemConfiguration"],
		"/system/configuration",
		{
			onError: () =>
				toast.error(
					"An error occurred while retrieving system configuration"
				),
		}
	);
