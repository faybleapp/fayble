import { ModalTabs } from "components/modalTabs";
import React, { useState } from "react";
import { Button, Container, Modal, Tab } from "react-bootstrap";

export const FirstRun = () => {
    const [activeTabKey, setActiveTabKey] = useState<string>("1");
	return (
		<>
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
									configuration to get Fayble up and running on your
									system. Click Next to get started.
								</p>
							</Container>
						</Tab>
						<Tab eventKey="2" title="Owner Account" disabled={true}>
							Test
						</Tab>
						<Tab eventKey="3" title="Configuration" disabled={true}>
							Test
						</Tab>
					</ModalTabs>
				</Modal.Body>
				<Modal.Footer>                    
					<Button variant="primary" onClick={() => setActiveTabKey((Number(activeTabKey) + 1).toString())}>Next</Button>
				</Modal.Footer>
			</Modal>
		</>
	);
};
