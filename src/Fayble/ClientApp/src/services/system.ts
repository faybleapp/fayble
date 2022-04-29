import { FirstRun } from "models/api-models";
import { useApiMutation } from "./useApiMutation";

export const useFirstRun = () => {
	return useApiMutation<null, null, FirstRun>(
		"POST",
		() => `/system/first-run`
	);
};
