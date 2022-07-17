import { Form } from "components/form";
import { TextField } from "components/form/textField";
import { isAuthenticated } from "helpers/authenticationHelpers";
import { LoginCredentials } from "models/api-models";
import { useEffect } from "react";
import { Button, Modal, Spinner } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { useLogin } from "services/authentication";
import styles from "./Login.module.scss";

export const Login = () => {
	const login = useLogin();
	const loggedIn = isAuthenticated();
	const navigate = useNavigate();

	useEffect(() => {
		if (loggedIn) {
			navigate("/");
		}
	}, [loggedIn, navigate]);

	const methods = useForm<LoginCredentials>();
	const watch = methods.watch();

	const submit: SubmitHandler<LoginCredentials> = (values) => {
		login.mutate([null, values], {
			onSuccess: () => {
				toast.success("Login successful");
				navigate("/");
			},
			onError: (error) => {
				switch (error.response?.status) {
					case 401:
						toast.error("Incorrect username or password");
						break;
					case 429:
						toast.error("Too many login attempts");
						break;
					default:
						toast.error("An error occurred while logging in");
				}
			},
		});
	};

	return (
		<>
			{!loggedIn && (
				<Modal show={true}>
					<Modal.Header className={styles.header}>
						<Modal.Title>LOG IN</Modal.Title>
					</Modal.Header>
					<Modal.Body>
						<Form<LoginCredentials>
							onSubmit={submit}
							methods={methods}
							className={styles.loginForm}>
							<TextField
								className={styles.loginField}
								name="username"
								placeholder="Username"
							/>
							<TextField
								className={styles.loginField}
								name="password"
								placeholder="Password"
								secure
							/>

							<Button
								className={styles.loginButton}
								type="submit"								
								disabled={
									!watch.username ||
									!watch.password ||
									login.isLoading
								}
								variant="primary">
								{login.isLoading ? (
									<Spinner
										as="span"
										animation="border"
										size="sm"
										role="status"
										aria-hidden="true"
									/>
								) : (
									"Login"
								)}
							</Button>
						</Form>
					</Modal.Body>
					<Modal.Footer></Modal.Footer>
				</Modal>
			)}
		</>
	);
};
