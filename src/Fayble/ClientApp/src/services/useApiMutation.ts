import { AxiosError } from "axios";
import { useMutation, UseMutationOptions } from "react-query";
import { useHttpClient } from "./httpClient";

export const useApiMutation = <Response, Variables, Data = null>(
	method: "PUT" | "POST" | "DELETE" | "PATCH",
	path: (variables: Variables) => string,	
	options?: UseMutationOptions<
		Response,
		AxiosError<Response>,
		[Variables, Data]
	>
) => {
	const httpClient = useHttpClient();

	return useMutation<Response, AxiosError<Response>, [Variables, Data]>(
		async ([variables, data]): Promise<Response> => {
			switch (method) {
				case "PATCH":
					return await httpClient.patch(path(variables), data);
				case "DELETE":
					return await httpClient.delete(path(variables), data);
				default:
					return await httpClient.post(path(variables), data);
			}
		},
		options
	);
};
