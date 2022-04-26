import { ModalTabs } from "components/modalTabs";
import { Library } from "models/api-models";
import React, { useState } from "react";
import { Container, Modal, Tab } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { useCreateLibrary, useDeleteLibrary, useUpdateLibrary } from "services";
import { LibraryDetailsTab } from "./LibraryDetailsTab";
import { LibraryModalFooter } from "./LibraryModalFooter";
import { LibraryPathsTab } from "./LibraryPathsTab";
import { LibrarySettingsTab } from "./LibrarySettingsTab";

interface LibraryModalProps {
	show: boolean;
	library?: Library;
	close: () => void;
}

const initialLibraryState: Library = {
	id: undefined,
	name: "",
	libraryType: "ComicBook",
	paths: [],
	settings: { reviewOnImport: true },
};

export const LibraryModal = ({ show, library, close }: LibraryModalProps) => {
	const [activeTabKey, setActiveTabKey] = useState<string>("1");
	const [isNew, setIsNew] = useState<boolean>(false);
	const [updatedLibrary, setUpdatedLibrary] =
		useState<Library>(initialLibraryState);
	const navigate = useNavigate();
	const deleteLibrary = useDeleteLibrary();
	const createLibrary = useCreateLibrary();
	const updateLibrary = useUpdateLibrary();

	const continueDisabled =
		activeTabKey === "1"
			? !updatedLibrary?.name?.trim()
			: updatedLibrary?.paths?.length === 0;

	const opened = () => {
		setIsNew(!library);
		setUpdatedLibrary(!library ? initialLibraryState : library);
	};

	const remove = () => {
		deleteLibrary.mutate([updatedLibrary.id!, null], {
			onSuccess: () => {
				close();
				navigate("/");
			},
		});
	};

	const create = () => {
		createLibrary.mutate([null, updatedLibrary], {
			onSuccess: () => {
				close();
			},
		});
	};

	const update = () => {
		updateLibrary.mutate([updatedLibrary.id!, updatedLibrary], {
			onSuccess: () => {
				close();
			},
		});
	};

	const tabsDisabled = continueDisabled || isNew;

	return (
		<Modal
			size="lg"
			show={show}
			onHide={close}
			onEntering={opened}
			onExited={() => setActiveTabKey("1")}>
			<Modal.Header>
				<Modal.Title>{isNew ? "New" : "Edit"} Library</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<ModalTabs
					onChange={(selectedTabKey) =>
						setActiveTabKey(selectedTabKey!)
					}
					defaultActiveKey="1"
					activeTab={isNew ? activeTabKey : undefined}>
					<Tab eventKey="1" title="Details" disabled={tabsDisabled}>
						<Container>
							<LibraryDetailsTab
								library={updatedLibrary}
								updateLibrary={setUpdatedLibrary}
								isNew={isNew}
							/>
						</Container>
					</Tab>
					<Tab eventKey="2" disabled={tabsDisabled} title="Paths">
						<Container>
							<LibraryPathsTab
								library={updatedLibrary}
								updateLibrary={setUpdatedLibrary}
							/>
						</Container>
					</Tab>
					<Tab
						eventKey="3"
						disabled={tabsDisabled}
						title="Configuration">
						<LibrarySettingsTab
							library={updatedLibrary}
							updateLibrary={setUpdatedLibrary}
							isNew={isNew}
						/>
					</Tab>
				</ModalTabs>
				<Modal.Footer>
					<LibraryModalFooter
						isNew={isNew}
						close={close}
						deleteLibrary={remove}
						createLibrary={create}
						updateLibrary={update}
						activeTabKey={activeTabKey}
						setActiveTabKey={setActiveTabKey}
						isDeleting={deleteLibrary.isLoading}
						isCreating={createLibrary.isLoading}
						isUpdating={updateLibrary.isLoading}
						continueDisabled={continueDisabled}
					/>
				</Modal.Footer>
			</Modal.Body>
		</Modal>
	);
};
