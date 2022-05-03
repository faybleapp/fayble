import { setAuthConfig } from "helpers/authenticationHelpers";
import { AuthenticationResult, LoginCredentials } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";

export const useLogin = () => {
    const queryClient = useQueryClient();
    return useApiMutation<AuthenticationResult, null, LoginCredentials>("POST", () => `/authentication/login`,
    {
        onSuccess: (result) => {              
           setAuthConfig(result);
           queryClient.invalidateQueries("currentUser")
        }
    });
};
