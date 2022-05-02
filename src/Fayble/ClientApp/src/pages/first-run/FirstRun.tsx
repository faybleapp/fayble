import { TextField } from "components/form/textField";
import { ModalTabs } from "components/modalTabs";
import { useFormik } from "formik";
import { FirstRun as FirstRunModel } from "models/api-models";
import React, { useEffect, useState } from "react";
import { Button, Container, Form, Modal, Spinner, Tab } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { useFirstRun } from "services/system";

export const FirstRun = () => {
	const [activeTabKey, setActiveTabKey] = useState<string>("1");
	const [allowContinue, setAllowContinue] = useState<boolean>(true);
	const [passwordConfirmation, setPasswordConfirmation] =
		useState<string>("");
	const [passwordValid, setPasswordValid] = useState<boolean>(true);

	const firstRun = useFirstRun();
	const navigate = useNavigate();

	const formik = useFormik<FirstRunModel>({
		initialValues: { ownerCredentials: { username: "", password: "" } },
		onSubmit: (values: FirstRunModel) => {
			firstRun.mutate([null, values], { onSuccess: () => navigate("/") });
		},
	});

	useEffect(() => {
		if (activeTabKey === "1") {
			setAllowContinue(true);
		} else if (activeTabKey === "2") {
			setAllowContinue(
				!!formik.values.ownerCredentials.username &&
					!!formik.values.ownerCredentials.password &&
					formik.values.ownerCredentials.password ===
						passwordConfirmation
			);
		}
	}, [formik.values, activeTabKey, passwordValid, passwordConfirmation]);

	useEffect(() => {
		setPasswordValid(
			formik.values.ownerCredentials.password === passwordConfirmation
		);
		if (!formik.values.ownerCredentials.password) {
			setPasswordConfirmation("");
		}
	}, [passwordConfirmation, formik.values.ownerCredentials.password]);

	const handleNext = () => {
		setActiveTabKey((Number(activeTabKey) + 1).toString());
	};

	return (
		<>
			<Modal size="lg" show={true}>
				<Modal.Header>
					<Modal.Title>Fayble</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form>
						<ModalTabs
							defaultActiveKey="1"
							activeTab={activeTabKey}>
							<Tab eventKey="1" title="Welcome" disabled={true}>
								<Container>
									<p>Welcome to Fayble!</p>
									<p>
										We will now run through some basic setup
										and configuration to get Fayble up and
										running on your system. Click Next to
										get started.
									</p>
								</Container>
							</Tab>
							<Tab
								eventKey="2"
								title="Owner Account"
								disabled={true}>
								<Container>
									<TextField
										name="ownerCredentials.username"
										label="Username"
										value={
											formik.values.ownerCredentials
												.username
										}
										onChange={formik.handleChange}
									/>
									<TextField
										name="ownerCredentials.password"
										secure
										label="Password"
										value={
											formik.values.ownerCredentials
												.password
										}
										onChange={formik.handleChange}
									/>
									{!!formik.values.ownerCredentials
										.password && (
										<TextField
											name="ownerCredentials.password"
											secure
											error={
												!passwordValid &&
												!!passwordConfirmation
													? "Passwords do not match"
													: ""
											}
											label="Confirm Password"
											value={passwordConfirmation}
											onChange={(
												event: React.ChangeEvent<HTMLInputElement>
											) =>
												setPasswordConfirmation(
													event.target.value
												)
											}
										/>
									)}
								</Container>
							</Tab>
							<Tab
								eventKey="3"
								title="Configuration"
								disabled={true}>
								Test
							</Tab>
						</ModalTabs>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					{Number(activeTabKey) > 1 && (
						<Button
							variant="secondary"
							disabled={firstRun.isLoading}
							onClick={() =>
								setActiveTabKey(
									(Number(activeTabKey) - 1).toString()
								)
							}>
							Back
						</Button>
					)}
					{Number(activeTabKey) === 3 ? (
						<Button
							variant="primary"
							disabled={firstRun.isLoading}
							onClick={formik.submitForm}>
							{firstRun.isLoading ? (
								<>
									<Spinner
										as="span"
										animation="border"
										size="sm"
										role="status"
										aria-hidden="true"
									/>
									{"  Saving..."}
								</>
							) : (
								"Finish"
							)}
						</Button>
					) : (
						<Button
							variant="primary"
							disabled={!allowContinue}
							onClick={handleNext}>
							Next
						</Button>
					)}
				</Modal.Footer>
			</Modal>
		</>
	);
};
