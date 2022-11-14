import { Button, Spinner } from "react-bootstrap";
import styles from "./LoadingButton.module.scss";

interface LoadingButtonProps {
  isLoading: boolean;
  loadingText?: string;
  variant?: "primary" | "secondary";
  text: string;
  className?: string;
  disabled?: boolean;
  onClick: () => void;
}

export const LoadingButton = ({
  isLoading,
  className,
  loadingText = "Loading...",
  text,
  variant = "primary",
  disabled = false,
  onClick,
}: LoadingButtonProps) => {
  return (
    <Button
    className={className}
      variant={variant}
      onClick={onClick}
      disabled={isLoading || disabled}>
      {isLoading ? (
        <>
          <Spinner
            className={styles.spinner}
            as="span"
            animation="border"
            size="sm"
            role="status"
            aria-hidden="true"
          />
          {loadingText}
        </>
      ) : (
        <>{text}</>
      )}
    </Button>
  );
};
