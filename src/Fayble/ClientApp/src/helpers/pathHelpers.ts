
export const SanitisePath = (string: string) =>
  string.trim().replace(/\/+$/, "").replace(/\\+$/, "").trim();

