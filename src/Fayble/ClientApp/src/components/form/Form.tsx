import { Form as RBForm } from "react-bootstrap";
import { FormProvider, SubmitHandler, UseFormReturn } from "react-hook-form";

interface FormProps<T> {
	methods: UseFormReturn<T, object>;
	onSubmit?: SubmitHandler<T>;
	className?: string;
	children?: React.ReactNode;
}

export const Form = <T extends object>({
	methods,
	onSubmit,
	className,
	children,
}: FormProps<T>) => {
	return (
		<FormProvider {...methods}>
			<RBForm
				className={className}
				onSubmit={onSubmit && methods.handleSubmit(onSubmit)}>
				{children}
			</RBForm>
		</FormProvider>
	);
};
