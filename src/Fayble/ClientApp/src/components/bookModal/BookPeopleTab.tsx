import { MultiSelectField } from "components/form/multiSelectField";
import { FormikProps } from "formik";
import { Book, BookPerson, Person } from "models/api-models";
import { RoleType, SelectFieldOption } from "models/ui-models";
import { Container, Form } from "react-bootstrap";
import { MultiValue } from "react-select";
import { usePeople } from "services/person";
interface BookPeopleTabProps {
	formik: FormikProps<Book>;
}

export const BookPeopleTab = ({ formik }: BookPeopleTabProps) => {
	var people = usePeople();
	var peopleOptions =
		Array.from(
			new Set(
				people?.data
					?.map((person: Person) => person.name)
					.concat(
						formik?.values?.people?.map((person) => person.name) ||
							[]
					) || []
			)
		).map((person) => ({
			value: person,
			label: person,
		})) || [];

	const onChange = (
		role: RoleType,
		selectedValues: MultiValue<SelectFieldOption> | null
	) => {
		const newPeople =
			selectedValues?.map((option) => ({
				name: option.value,
				role: role,
			})) || [];
		formik.setFieldValue(
			"people",
			(formik.values.people || [])
				.filter((people) => people.role !== role)
				.concat(newPeople as BookPerson[])
		);
	};

	return (
		<Container>
			<Form>
				<MultiSelectField
					name="writers"
					creatable
					clearable
					locked={formik.values.fieldLocks.writers}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.writers", lock)
					}
					value={
						formik.values.people
							?.filter(
								(person) => person.role === RoleType.Writer
							)
							.map((person) => person.name) || []
					}
					label="Writers"
					onChange={(selectedValues) => {
						onChange(RoleType.Writer, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="inkers"
					creatable
					clearable
					locked={formik.values.fieldLocks.inkers}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.inkers", lock)
					}
					value={
						formik.values.people
							?.filter((person) => person.role === RoleType.Inker)
							.map((person) => person.name) || []
					}
					label="Inkers"
					onChange={(selectedValues) => {
						onChange(RoleType.Inker, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="editors"
					creatable
					clearable
					locked={formik.values.fieldLocks.editors}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.editors", lock)
					}
					value={
						formik.values.people
							?.filter(
								(person) => person.role === RoleType.Editor
							)
							.map((person) => person.name) || []
					}
					label="Editors"
					onChange={(selectedValues) => {
						onChange(RoleType.Editor, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="pencillers"
					creatable
					clearable
					locked={formik.values.fieldLocks.pencillers}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.pencillers", lock)
					}
					value={
						formik.values.people
							?.filter(
								(person) => person.role === RoleType.Penciller
							)
							.map((person) => person.name) || []
					}
					label="Pencillers"
					onChange={(selectedValues) => {
						onChange(RoleType.Penciller, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="letterers"
					creatable
					clearable
					locked={formik.values.fieldLocks.letterers}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.letterers", lock)
					}
					value={
						formik.values.people
							?.filter(
								(person) => person.role === RoleType.Letterer
							)
							.map((person) => person.name) || []
					}
					label="Letterers"
					onChange={(selectedValues) => {
						onChange(RoleType.Letterer, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="colorists"
					creatable
					clearable
					locked={formik.values.fieldLocks.colorists}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.colorists", lock)
					}
					value={
						formik.values.people
							?.filter(
								(person) => person.role === RoleType.Colorist
							)
							.map((person) => person.name) || []
					}
					label="Colorists"
					onChange={(selectedValues) => {
						onChange(RoleType.Colorist, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="coverArtists"
					creatable
					clearable
					locked={formik.values.fieldLocks.coverArtists}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.coverArtists", lock)
					}
					value={
						formik.values.people
							?.filter(
								(person) => person.role === RoleType.CoverArtist
							)
							.map((person) => person.name) || []
					}
					label="Cover Artists"
					onChange={(selectedValues) => {
						onChange(RoleType.CoverArtist, selectedValues);
					}}
					options={peopleOptions}
				/>
				<MultiSelectField
					name="other"
					creatable
					clearable
					locked={formik.values.fieldLocks.other}
					onLock={(lock: boolean) =>
						formik.setFieldValue("fieldLocks.other", lock)
					}
					value={
						formik.values.people
							?.filter((person) => person.role === RoleType.Other)
							.map((person) => person.name) || []
					}
					label="Other"
					onChange={(selectedValues) => {
						onChange(RoleType.Other, selectedValues);
					}}
					options={peopleOptions}
				/>
			</Form>
		</Container>
	);
};
