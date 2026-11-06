import { AfterViewInit, Directive, ElementRef, inject, OnDestroy } from '@angular/core';

/**
 * Animates this element's height to follow its first child, so content of a different
 * size swaps in smoothly instead of making the page jump.
 */
@Directive({
  selector: '[appAutoHeight]',
  host: { class: 'block overflow-hidden transition-[height] duration-300 ease-out' },
})
export class AutoHeight implements AfterViewInit, OnDestroy {
  private readonly host = inject<ElementRef<HTMLElement>>(ElementRef).nativeElement;
  private observer?: ResizeObserver;

  ngAfterViewInit(): void {
    const content = this.host.firstElementChild;
    if (!content) {
      return;
    }
    this.observer = new ResizeObserver(() => {
      this.host.style.height = `${(content as HTMLElement).offsetHeight}px`;
    });
    this.observer.observe(content);
  }

  ngOnDestroy(): void {
    this.observer?.disconnect();
  }
}
