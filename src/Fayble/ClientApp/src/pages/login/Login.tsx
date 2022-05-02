import { TextField } from "components/form/textField";
import { useFormik } from "formik";
import { LoginCredentials } from "models/api-models";
import React from "react";
import { Form } from "react-bootstrap";
import { useLogin } from "services/authentication";

interface LoginProps {}

export const Login = ({}: LoginProps) => {
	const login = useLogin();

	const formik = useFormik<LoginCredentials>({
		initialValues: { username: "", password: "" },
		enableReinitialize: true,
		onSubmit: (values: LoginCredentials) => {
			login.mutate([null, values], {
				onSuccess: () => {
					// close();
				},
			});
		},
		validateOnMount: true,
	});

	return (
		<>
			<Form>
				<TextField
					name="username"
                    label="Username"
					value={formik.values.username}
					onChange={formik.handleChange}
				/>
			</Form>
		</>
	);
};
