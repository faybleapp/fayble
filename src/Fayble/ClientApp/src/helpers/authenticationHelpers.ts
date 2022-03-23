import { Authentication } from "models/api-models";

export const getAuthConfig = (): Authentication => {
	return JSON.parse(
		localStorage.getItem("authentication") || "{}"
	) as Authentication;
};

export const isLoggedIn = (): boolean => {
	const config = getAuthConfig();
	return config !== null && config.loggedIn;
};

export const setAuthConfig = (auth: Authentication) => {
	localStorage.setItem("authentication", JSON.stringify(auth));
};

export const clearAuthConfig = () => {
	localStorage.removeItem("authentication");
};
