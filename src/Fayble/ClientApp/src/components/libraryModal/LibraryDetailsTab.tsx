import { SelectField } from "components/form/selectField";
import { TextField } from "components/form/textField";
import { Library } from "models/api-models";
import { Form } from "react-bootstrap";

interface LibraryDetailsTabProps {
	library: Library;
	isNew: boolean;
	updateLibrary: (library: Library) => void;
}

export const LibraryDetailsTab = ({
	library,
	isNew,
	updateLibrary,
}: LibraryDetailsTabProps) => {
    
	const handleInputChange = (name: string, value: string): void => {
		if (name === "libraryType") {
			updateLibrary({ ...library, libraryType: value });
		} else {
			updateLibrary({ ...library, [name]: value });
		}
	};

	return (
		<Form>
			<SelectField
				name="libraryType"
				label="Library Type"
				value={library?.libraryType}
				disabled={!isNew}
				onChange={(selectedValue) =>
					handleInputChange("libraryType", selectedValue as string)
				}
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
			<TextField
				name="name"
				label="Library Name"				
				value={library?.name}
				onChange={(e) =>
					handleInputChange(
						e.currentTarget.name,
						e.currentTarget.value
					)
				}
			/>
		</Form>
	);
};
