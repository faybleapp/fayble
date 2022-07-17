import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";

interface LibraryDetailsTabProps {
	isNew: boolean;
}

export const LibraryDetailsTab = ({ isNew }: LibraryDetailsTabProps) => {
	return (
		<>
			<SelectField
				name="libraryType"
				label="Library Type"
				disabled={!isNew}
				options={[
					{
						value: "ComicBook",
						label: "Comic Book",
					},
					{
						value: "Book",
						label: "Book",
					},
				]}
			/>
			<TextField name="name" label="Library Name" />
		</>
	);
};
