import { SwitchField } from "components/form/switchField";
import { Library } from "models/api-models";
import React from "react";
import { Container, Form } from "react-bootstrap";
import styles from "./LibrarySettingsTab.module.scss";

interface LibrarySettingsTabProps {
	library: Library;
	isNew: boolean;
	updateLibrary: (library: Library) => void;
}

export const LibrarySettingsTab = ({
	library,
	isNew,
	updateLibrary,
}: LibrarySettingsTabProps) => {
	
	const handleChange = <T,>(name: string, value: T): void => {
		const updatedSettings = { ...library.settings, [name]: value };
		updateLibrary({ ...library, settings: updatedSettings });
	};

	return (
		<Container>
			<Form>
				<SwitchField
					name="reviewOnImport"
					label="Review on Import"
					value={library.settings.reviewOnImport}
					onChange={(e) => {
						handleChange<boolean>(e.target.name, e.target.checked)
					}}
				/>
				<p className={styles.settingDetail}>
					All new {library.libraryType.toLowerCase()}s are added to
					the 'Review List' for review prior to being imported into
					the library.
				</p>
			</Form>
		</Container>
	);
};
