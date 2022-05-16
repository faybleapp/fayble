import { TextField } from "components/form/textField";
import { useFormik } from "formik";
import { isAuthenticated } from "helpers/authenticationHelpers";
import { LoginCredentials } from "models/api-models";
import { useEffect } from "react";
import { Button, Form, Modal, Spinner } from "react-bootstrap";
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

	const formik = useFormik<LoginCredentials>({
		initialValues: { username: "", password: "" },
		enableReinitialize: true,
		onSubmit: (values: LoginCredentials) => {
			login.mutate([null, values], {
				onSuccess: () => {
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
		},
		validateOnMount: true,
	});

	return (
		<>
			{!loggedIn && (
				<Modal show={true}>
					<Modal.Header className={styles.header}>
						<Modal.Title>LOG IN</Modal.Title>
					</Modal.Header>
					<Modal.Body>
						<Form className={styles.loginForm}>
							<TextField
								className={styles.loginField}
								name="username"
								placeholder="Username"
								value={formik.values.username}
								onChange={formik.handleChange}
							/>
							<TextField
								className={styles.loginField}
								name="password"
								placeholder="Password"
								value={formik.values.password}
								secure
								onChange={formik.handleChange}
							/>
						</Form>
						<Button
							className={styles.loginButton}
							onClick={formik.submitForm}
							disabled={
								!formik.values.username ||
								!formik.values.password ||
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
					</Modal.Body>
					<Modal.Footer></Modal.Footer>
				</Modal>
			)}
		</>
	);
};
