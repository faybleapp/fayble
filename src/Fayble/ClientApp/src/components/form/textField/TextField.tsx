import cn from "classnames";
import { FieldLock } from "components/fieldLock";
import { LockableField } from "models/ui-models";
import { Form, InputGroup } from "react-bootstrap";
import { useFormContext } from "react-hook-form";

interface TextFieldProps extends LockableField {
	name: string;
	label?: string;
	className?: string;
	secure?: boolean;
	placeholder?: string;
	lockable?: boolean;
}

export const TextField = ({
	name,
	label,
	className,
	lockable,
	secure,
	placeholder,	
}: TextFieldProps) => {
	const {
		register,
		setValue,
		watch,
		formState: { errors },
	} = useFormContext();

	const locked = watch(`fieldLocks.${name}`);
	const field = register(name);
	return (
		<Form.Group className={cn(className, "mb-3")}>
			{label && <Form.Label>{label}</Form.Label>}
			<InputGroup>
				<Form.Control
					type={secure ? "password" : "text"}
					isInvalid={!!errors.name}
					{...field}
					onChange={(e: any) => {
						if (lockable) {
							setValue(`fieldLocks.${name}`, true);
						}
						field.onChange(e);
					}}
					placeholder={placeholder}
				/>
				{lockable && (
					<FieldLock
						locked={locked}
						onClick={() => setValue(`fieldLocks.${name}`, !locked)}
					/>
				)}
				<Form.Control.Feedback type="invalid">
					{errors.name?.message?.toString()}
				</Form.Control.Feedback>
			</InputGroup>
		</Form.Group>
	);
};
