export const SanitisePaths = (string: string) =>
	string.trim().replace(/\/+$/, "").replace(/\\+$/, "").trim();
