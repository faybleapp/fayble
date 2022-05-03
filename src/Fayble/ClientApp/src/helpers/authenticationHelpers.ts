import { AuthenticationResult } from "models/api-models";

export const getAuthConfig = (): AuthenticationResult => {
	return JSON.parse(
		localStorage.getItem("authentication") || "{}"
	) as AuthenticationResult;
};

export const isAuthenticated = (): boolean => {
	const config = getAuthConfig();	
	return config?.isAuthenticated === true;
};

export const setAuthConfig = (auth: AuthenticationResult) => {
	localStorage.setItem("authentication", JSON.stringify(auth));
};

export const clearAuthConfig = () => {
	localStorage.removeItem("authentication");
};
