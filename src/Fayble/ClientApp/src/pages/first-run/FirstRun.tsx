import { yupResolver } from "@hookform/resolvers/yup";
import { Form } from "components/form";
import { TextField } from "components/form/textField";
import { ModalTabs } from "components/modalTabs";
import { FirstRun as FirstRunModel } from "models/ui-models";
import { useEffect, useState } from "react";
import { Button, Container, Modal, Spinner, Tab } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useFirstRun, useSystemConfiguration } from "services/system";
import * as yup from "yup";

export const FirstRun = () => {
  const [activeTabKey, setActiveTabKey] = useState<string>("1");
  const { data: systemConfiguration } = useSystemConfiguration();

  const firstRun = useFirstRun();
  const navigate = useNavigate();

  const validationSchema = yup.object().shape({
    username: yup.string().required("Username is required"),
    password: yup.string().required("A password is required"),
    passwordConfirmation: yup
      .string()
      .oneOf([yup.ref("password"), null], "Passwords must match"),
  });

  const form = useForm<FirstRunModel>({
    mode: "onChange",
    resolver: yupResolver(validationSchema),
    defaultValues: { username: "", password: "", passwordConfirmation: "" },
  });

  const formFields = form.watch();

  const handleSubmit: SubmitHandler<FirstRunModel> = (values) => {
    firstRun.mutate(
      {
        ownerCredentials: {
          username: values.username,
          password: values.password,
        },
      },
      {
        onSuccess: () => navigate("/login"),
      }
    );
  };

  useEffect(() => {
    if (!systemConfiguration?.firstRun) navigate("/");
  }, [systemConfiguration, navigate]);

  const AllowNavigation = () => {
    if (firstRun.isLoading) {
      return false;
    }

    switch (activeTabKey) {
      case "1":
        return true;
      case "2":
        return form.formState.isValid;
      case "3":
        return true;
    }
  };

  const handleNext = () => {
    if (AllowNavigation()) {
      setActiveTabKey((Number(activeTabKey) + 1).toString());
    }
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
                  We will now run through some basic setup and configuration to
                  get Fayble up and running on your system. Click Next to get
                  started.
                </p>
              </Container>
            </Tab>
            <Tab eventKey="2" title="Owner Account" disabled={true}>
              <Container>
                <TextField name="username" label="Username" />
                <TextField name="password" secure label="Password" />
                {!!formFields.password && (
                  <TextField
                    name="passwordConfirmation"
                    secure
                    label="Confirm Password"
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
              disabled={!AllowNavigation()}
              onClick={() =>
                setActiveTabKey((Number(activeTabKey) - 1).toString())
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
              disabled={!AllowNavigation()}
              onClick={handleNext}>
              Next
            </Button>
          )}
        </Modal.Footer>
      </Modal>
    </Form>
  );
};
