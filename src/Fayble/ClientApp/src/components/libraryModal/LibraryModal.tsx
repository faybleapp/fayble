import { Form } from "components/form";
import { ModalTabs } from "components/modalTabs";
import { Library } from "models/api-models";
import { useState } from "react";
import { Container, Modal, Tab } from "react-bootstrap";
import { SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useCreateLibrary, useDeleteLibrary, useUpdateLibrary } from "services";
import { LibraryDetailsTab } from "./LibraryDetailsTab";
import { LibraryModalFooter } from "./LibraryModalFooter";
import { LibraryPathTab } from "./LibraryPathTab";
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
	folderPath: "",
	settings: {
		reviewOnImport: false,
		useComicInfo: false,
		yearAsVolume: false,
	},
};

export const LibraryModal = ({ show, library, close }: LibraryModalProps) => {
	const [activeTabKey, setActiveTabKey] = useState<string>("1");
	
	const navigate = useNavigate();
	const deleteLibrary = useDeleteLibrary();
	const createLibrary = useCreateLibrary();
	const updateLibrary = useUpdateLibrary();	
	const form = useForm<Library>({
		defaultValues: !library ? initialLibraryState : library,
	});

	const name = form.watch("name");
	const folderPath = form.watch("folderPath");

	const continueDisabled = activeTabKey === "1" ? !name?.trim() : !folderPath;
	const isNew = !library;
	const tabsDisabled = continueDisabled || isNew;

	const opened = () => {		
		form.reset(!library ? initialLibraryState : library);
	};

	const remove: SubmitHandler<Library> = (library) => {
		deleteLibrary.mutate([library.id!, null], {
			onSuccess: () => {
				close();
				navigate("/");
			},
		});
	};

	const create: SubmitHandler<Library> = (library) => {
		createLibrary.mutate([null, library], {
			onSuccess: () => {
				close();
			},
		});
	};

	const update: SubmitHandler<Library> = (library) => {
		updateLibrary.mutate([library.id!, library], {
			onSuccess: () => {
				close();
			},
		});
	};

	
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
				<Form<Library> form={form}>
					<ModalTabs
						onChange={(selectedTabKey) =>
							setActiveTabKey(selectedTabKey!)
						}
						defaultActiveKey="1"
						activeTab={isNew ? activeTabKey : undefined}>
						<Tab
							eventKey="1"
							title="Details"
							disabled={tabsDisabled}>
							<Container>
								<LibraryDetailsTab isNew={isNew} />
							</Container>
						</Tab>
						<Tab
							eventKey="2"
							disabled={tabsDisabled}
							title="Folder Path">
							<Container>
								<LibraryPathTab />
							</Container>
						</Tab>
						<Tab
							eventKey="3"
							disabled={tabsDisabled}
							title="Configuration">
							<LibrarySettingsTab />
						</Tab>
					</ModalTabs>
				</Form>
				<Modal.Footer>
					<LibraryModalFooter
						isNew={isNew}
						close={close}
						deleteLibrary={form.handleSubmit(remove)}
						createLibrary={form.handleSubmit(create)}
						updateLibrary={form.handleSubmit(update)}
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
