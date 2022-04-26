import { User } from "models/api-models";
import { useApiQuery } from "./useApiQuery";

export const useUser = () =>
	useApiQuery<User>(["user"], `/users/current`);