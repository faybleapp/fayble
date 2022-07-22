import { SwitchField } from "components/form/switchField";
import { Container } from "react-bootstrap";
import styles from "./LibrarySettingsTab.module.scss";

export const LibrarySettingsTab = () => {
	return (
		<Container>
			<SwitchField
				name="settings.reviewOnImport"
				label="Review on import"
				className={styles.setting}
			/>
			<p className={styles.settingDetail}>
				All new books are added to the 'Review List' for review prior to
				being imported into the library.
			</p>
			<SwitchField
				name="settings.useComicInfo"
				label="Use ComicInfo"
				className={styles.setting}
			/>
			<p className={styles.settingDetail}>
				Enable this setting to read metadata from the ComicInfo XML file
				if present.
			</p>
			<SwitchField
				name="settings.yearAsVolume"
				label="Use year as volume"
				className={styles.setting}
			/>
			<p className={styles.settingDetail}>
				When enabled, the series volume value will default to the series
				year when populated from metadata or ComicInfo.
			</p>
		</Container>
	);
};
