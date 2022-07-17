import { Form } from "react-bootstrap";
import { useFormContext } from "react-hook-form";

interface SwitchFieldProps {
	name: string;
	label: string;
	className?: string;
}

export const SwitchField = ({
	name,
	label,
	className,
}: SwitchFieldProps) => {

	const {register} = useFormContext();
	return (		
			<Form.Check
				{...register(name)}
				type="switch"               
                className={className}
				label={label}
			/>                             
		
	);
};
