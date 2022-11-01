import { faCircleQuestion } from "@fortawesome/free-regular-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { OverlayTrigger, Tooltip as BSTooltip } from "react-bootstrap";
import styles from "./Tooltip.module.scss";

interface TooltipProps {
  tooltip: string;
}

export const Tooltip = ({ tooltip }: TooltipProps) => {
  return (
    <OverlayTrigger
      placement="right"
      delay={{ show: 0, hide: 400 }}
      overlay={<BSTooltip>{tooltip}</BSTooltip>}>
      <div className={styles.tooltip}>        
        <FontAwesomeIcon icon={faCircleQuestion} />
      </div>
    </OverlayTrigger>
  );
};
