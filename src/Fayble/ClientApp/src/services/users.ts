import { User } from "models/api-models";
import { useApiQuery } from "./useApiQuery";

export const useCurrentUser = () =>
	useApiQuery<User>(["currentUser"], `/users/current`);