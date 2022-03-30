import { SelectFieldOption } from "models/ui-models";
import React from "react";
import { Form } from "react-bootstrap";
import Select, { ActionMeta, MultiValue, StylesConfig } from "react-select";
import CreatableSelect from "react-select/creatable";
import styles from "./MultiSelectField.module.scss";

interface SelectFieldProps {
	name: string;
	label: string;
	className?: string;
	creatable?: boolean;
	disabled?: boolean;
	clearable?: boolean;
	searchable?: boolean;
	value: string[];
	placeholder?: string;
	options: SelectFieldOption[] | [];
	onChange?: (
		options: MultiValue<SelectFieldOption> | null,
		action: ActionMeta<OptionTypeBase>
	) => void;
}

type OptionTypeBase =
	| {
			label: string;
			value: string;
			// eslint-disable-next-line @typescript-eslint/no-explicit-any
	  }
	| any;

export const MultiSelectField = ({
	label,
	name,
	creatable,
	disabled,
	className,
	clearable,
	searchable,
	value,
	options,
	onChange,
}: SelectFieldProps) => {
	const selectStyle: StylesConfig<OptionTypeBase, boolean> = {
		control: (provided) => ({
			...provided,
			backgroundColor: styles.background,
			border: styles.border,
			//boxShadow: styles.none,
		}),
		input: (provided) => ({
			...provided,
			color: styles.color,
		}),
		option: (provided) => ({
			...provided,
			backgroundColor: styles.secondaryBackground,
			color: styles.color,
			":hover": {
				color: styles.accentColor,
			},
			":active": {
				background: styles.none,
			},
		}),
		singleValue: (provided) => ({
			...provided,
			color: styles.color,
		}),
		valueContainer: (provided) => ({
			...provided,
			backgroundColor: styles.background,
		}),
		menuList: (provided) => ({
			...provided,
			backgroundColor: styles.secondaryBackground,
		}),
	};

	const handleIsValidNewOption = (inputValue: string) => {
		const exactValueExists = options.find(
			(option) => option.label.toLowerCase() === inputValue.toLowerCase()
		);

		const valueIsNotEmpty = inputValue.trim().length !== 0;
		return !exactValueExists && valueIsNotEmpty;
	};

	return (
		<Form.Group className="mb-3">
			<Form.Label>{label}</Form.Label>
			{creatable ? (
				<CreatableSelect
					isMulti
					name={name}
					isDisabled={disabled}
					className={className}
					styles={selectStyle}
					isClearable={clearable}
					isSearchable={searchable}
					defaultValue={null}
					value={
						options
							.filter(
								(option) => value.indexOf(option.label) >= 0
							) || []
					}
					options={options}
					onChange={onChange}
					isValidNewOption={handleIsValidNewOption}
				/>
			) : (
				<Select
					isMulti
					name={name}
					isDisabled={disabled}
					className={className}
					styles={selectStyle}
					isClearable={clearable}
					isSearchable={searchable}
					defaultValue={null}
					value={
						options.filter(
							(option) => value.indexOf(option.value) >= 0
						) || []
					}
					options={options}
					onChange={onChange}
				/>
			)}
		</Form.Group>
	);
};
