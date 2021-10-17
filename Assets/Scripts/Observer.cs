// 옵저버 추상클래스
// : 옵저버들이 구현해야 할 인터페이스 메서드

public abstract class Observer
{
    // 상태 Update 메서드
    public abstract void OnNotify(int num);
}
