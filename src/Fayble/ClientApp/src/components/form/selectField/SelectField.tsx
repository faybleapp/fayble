import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField, SelectFieldOption } from "models/ui-models";
import { Form, InputGroup } from "react-bootstrap";
import Select, { ActionMeta, StylesConfig } from "react-select";
import styles from "./SelectField.module.scss";

interface SelectFieldProps extends LockableField {
	name: string;
	label: string;
	className?: string;
	disabled?: boolean;
	clearable?: boolean;
	searchable?: boolean;
	value?: string;
	placeholder?: string;
	options: SelectFieldOption[] | [];
	onChange?: (
		value: string | any[] | null | undefined,
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

export const SelectField = ({
	label,
	name,
	disabled,
	className,
	clearable,
	searchable,
	value,
	options,
	onChange,
	onLock,
	locked = false,
}: SelectFieldProps) => {
	const handleChange = (
		option: SelectFieldOption | null,
		action: ActionMeta<OptionTypeBase>
	) => {
		if (onChange) {
			const value =
				option instanceof Array
					? option.map((o) => o.value)
					: option?.value;

			onChange(value, action);
		}
	};

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

	return (
		<Form.Group className="mb-3">
			<Form.Label>{label}</Form.Label>
			<InputGroup>
				<Select
					name={name}
					isDisabled={disabled === true}
					className={cn(className, styles.select, {
						[styles.lockable]: onLock,
					})}
					styles={selectStyle}
					isClearable={clearable === true}
					isSearchable={searchable === true}
					defaultValue={null}
					value={options.find((option) => option.value === value)}
					options={options}
					onChange={(e: any, a: any) => {
						if (onLock){
							onLock(true);
						}
						handleChange(e, a);
					}}
				/>
				{onLock && <FieldLock locked={locked} onClick={onLock} />}
			</InputGroup>
		</Form.Group>
	);
};
