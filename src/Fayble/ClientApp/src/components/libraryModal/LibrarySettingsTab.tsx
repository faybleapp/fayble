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
		console.log(library);
	};

	return (
		<Container>
			<Form>
				<SwitchField
					name="reviewOnImport"
					label="Review on import"
					className={styles.setting}
					value={library.settings.reviewOnImport}
					onChange={(e) => {
						handleChange<boolean>(e.target.name, e.target.checked);
					}}
				/>
				<p className={styles.settingDetail}>
					All new books are added to the 'Review List' for review
					prior to being imported into the library.
				</p>
				<SwitchField
					name="seriesFolders"
					label="Series folders"
					className={styles.setting}
					value={library.settings.seriesFolders}
					onChange={(e) => {
						handleChange<boolean>(e.target.name, e.target.checked);
					}}
				/>
				<p className={styles.settingDetail}>
					Enable this setting if your books reside in a parent series
					folder, e.g. 'Star Wars (2015)'. This folder will be used to
					determine the series and year. Alternatively, disable
					this setting to retrieve the series and year from the
					filename, e.g. 'Star Wars Vol. 2015 #001 (March 2015)'
				</p>
			</Form>
		</Container>
	);
};
