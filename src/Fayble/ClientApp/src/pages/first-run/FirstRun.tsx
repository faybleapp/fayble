import { Form } from "components/form";
import { OldTextField, TextField } from "components/form/textField";
import { ModalTabs } from "components/modalTabs";
import { FirstRun as FirstRunModel } from "models/api-models";
import React, { useEffect, useState } from "react";
import { Button, Container, Modal, Spinner, Tab } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useFirstRun, useSystemConfiguration } from "services/system";

export const FirstRun = () => {
	const [activeTabKey, setActiveTabKey] = useState<string>("1");
	const [allowContinue, setAllowContinue] = useState<boolean>(true);
	const [passwordConfirmation, setPasswordConfirmation] =
		useState<string>("");
	const [passwordValid, setPasswordValid] = useState<boolean>(true);
	const { data: systemConfiguration } = useSystemConfiguration();

	const firstRun = useFirstRun();
	const navigate = useNavigate();

	const form = useForm<FirstRunModel>({
		defaultValues: { ownerCredentials: { username: "", password: "" } },
	});

	const formFields = form.watch();

	const handleSubmit: SubmitHandler<FirstRunModel> = (values) => {
		firstRun.mutate([null, values], {
			onSuccess: () => navigate("/login"),
		});
	};

	useEffect(() => {
		if (!systemConfiguration?.firstRun) navigate("/");
	}, [systemConfiguration, navigate]);

	useEffect(() => {
		if (activeTabKey === "1") {
			setAllowContinue(true);
		} else if (activeTabKey === "2") {
			setAllowContinue(
				!!formFields.ownerCredentials.username &&
					!!formFields.ownerCredentials.password &&
					formFields.ownerCredentials.password ===
						passwordConfirmation
			);
		}
	}, [formFields, activeTabKey, passwordValid, passwordConfirmation]);

	useEffect(() => {
		setPasswordValid(
			formFields.ownerCredentials.password === passwordConfirmation
		);
		if (!formFields.ownerCredentials.password) {
			setPasswordConfirmation("");
		}
	}, [passwordConfirmation, formFields.ownerCredentials.password]);

	const handleNext = () => {
		setActiveTabKey((Number(activeTabKey) + 1).toString());
	};

	return (
		<Form<FirstRunModel> form={form}>
			<Modal size="lg" show={true}>
				<Modal.Header>
					<Modal.Title>Fayble</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<ModalTabs defaultActiveKey="1" activeTab={activeTabKey}>
						<Tab eventKey="1" title="Welcome" disabled={true}>
							<Container>
								<p>Welcome to Fayble!</p>
								<p>
									We will now run through some basic setup and
									configuration to get Fayble up and running
									on your system. Click Next to get started.
								</p>
							</Container>
						</Tab>
						<Tab eventKey="2" title="Owner Account" disabled={true}>
							<Container>
								<TextField
									name="ownerCredentials.username"
									label="Username"
								/>
								<TextField
									name="ownerCredentials.password"
									secure
									label="Password"
								/>
								{!!formFields.ownerCredentials.password && (
									<OldTextField
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
						<Tab eventKey="3" title="Configuration" disabled={true}>
							Test
						</Tab>
					</ModalTabs>
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
							onClick={form.handleSubmit(handleSubmit)}>
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
		</Form>
	);
};
