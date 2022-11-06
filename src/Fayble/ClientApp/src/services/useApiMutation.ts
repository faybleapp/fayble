import { AxiosError } from "axios";
import { useMutation, UseMutationOptions } from "react-query";
import { useHttpClient } from "./httpClient";

export const useApiMutation = <Response, Data = null>(
  method: "PUT" | "POST" | "DELETE" | "PATCH",
  path: (data: Data) => string,
  options?: UseMutationOptions<Response, AxiosError<Response>, Data>
) => {
  const httpClient = useHttpClient();

  return useMutation<Response, AxiosError<Response>, Data>(
    async (data): Promise<Response> => {
      switch (method) {
        case "PATCH":
          return (await httpClient.patch(path(data), data)).data;
        case "DELETE":
          return (await httpClient.delete(path(data))).data;
        default:
          return (await httpClient.post(path(data), data)).data;
      }
    },
    options
  );
};
