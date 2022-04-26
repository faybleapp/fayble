import { AuthenticationResult } from "models/api-models";

export const getAuthConfig = (): AuthenticationResult => {
	return JSON.parse(
		localStorage.getItem("authentication") || "{}"
	) as AuthenticationResult;
};

export const isLoggedIn = (): boolean => {
	const config = getAuthConfig();
	return config !== null && config.loggedIn;
};

export const setAuthConfig = (auth: AuthenticationResult) => {
	localStorage.setItem("authentication", JSON.stringify(auth));
};

export const clearAuthConfig = () => {
	localStorage.removeItem("authentication");
};
