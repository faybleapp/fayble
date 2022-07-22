import { Form as RBForm } from "react-bootstrap";
import { FormProvider, SubmitHandler, UseFormReturn } from "react-hook-form";

interface FormProps<T> {
	form: UseFormReturn<T, object>;
	onSubmit?: SubmitHandler<T>;
	className?: string;
	children?: React.ReactNode;
}

export const Form = <T extends object>({
	form,
	onSubmit,
	className,
	children,
}: FormProps<T>) => {
	return (
		<FormProvider {...form}>
			<RBForm
				className={className}
				onSubmit={onSubmit && form.handleSubmit(onSubmit)}>
				{children}
			</RBForm>
		</FormProvider>
	);
};
