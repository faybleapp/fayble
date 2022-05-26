import { SwitchField } from "components/form/switchField";
import { Library } from "models/api-models";
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
					name="useComicInfo"
					label="Use ComicInfo"
					className={styles.setting}
					value={library.settings.useComicInfo}
					onChange={(e) => {
						handleChange<boolean>(e.target.name, e.target.checked);
					}}
				/>
				<p className={styles.settingDetail}>
					Enable this setting to read metadata from the ComicInfo XML
					file if present.
				</p>
				<SwitchField
					name="yearAsVolume"
					label="Use year as volume"
					className={styles.setting}
					value={library.settings.yearAsVolume}
					onChange={(e) => {
						handleChange<boolean>(e.target.name, e.target.checked);
					}}
				/>
				<p className={styles.settingDetail}>
					When enabled, the series volume value will default to the
					series year when populated from metadata or ComicInfo.
				</p>
			</Form>
		</Container>
	);
};
